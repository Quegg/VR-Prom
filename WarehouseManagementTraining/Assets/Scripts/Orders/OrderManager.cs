
using System;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [Header("There needs to be at least one small item in every order")]
        public GameObject palletPlaces;

        public GameObject palletPlacePrefab;

        private float lastCheckButtonPress;
        public float buttonPressDelay = 1f;

        [Header("Check automatically if orders are ready")]
        public bool checkOrdersAutomatically;

        [SerializeField] private GameObject firstPoint;

        [SerializeField] private GameObject workstation;

        //[SerializeField] private GameObject boxPrefab;
        [SerializeField] private GameObject pickingCartPrefab;

        [Header("true= positive x direction; false negative")] [SerializeField]
        private bool direction;

        [SerializeField] private int maxBoxes;

        [Header("How much space (in x) needs one box?")] [SerializeField]
        private float boxOffset;

        [Header("Displays to display, if order is complete")] [SerializeField]
        private GameObject displaysObject;

        public GameObject SelectionCanvas;
        private ShowOrderMenuItems canvasScript;

        private Displays displays;

        //order[x] coresponds to shippinbox[x] corresponds to itemsNeeded[x]
        private Order[] orders;
        private PickingCart[] boxes;
        private Dictionary<string, int>[] itemsNeeded;
        private bool[] orderComplete;
        private List<BigItemInOrder> bigItemsinOrders;

        public AudioSource successSound;
        public AudioSource failSound;

        private GuidingController guidingController;
        [HideInInspector] public int selectedOrderIdExtern = -1;

        public Sprite[] barcodeSprites;

        private GameObject lastPlacedSmallObject;

        public GameObject LastPlacedSmallObject
        {
            get => lastPlacedSmallObject;
            set => lastPlacedSmallObject = value;
        }


        
        void Start()
        {
            guidingController=FindObjectOfType<GuidingController>();
            lastCheckButtonPress = Time.time;
            bigItemsinOrders = new List<BigItemInOrder>();
            canvasScript = SelectionCanvas.GetComponent<ShowOrderMenuItems>();
            canvasScript.orderManager = this;
            
            float currentOffset = 0;
            orders = GetComponentsInChildren<Order>();
            if (orders.Length > maxBoxes)
                throw new Exception("maximum " + maxBoxes +
                                    " order-boxes Supported please check your orders or adjust the maxBoxes parameter");

            orderComplete = new bool[orders.Length];
            boxes = new PickingCart[orders.Length];
            itemsNeeded = new Dictionary<string, int>[orders.Length];
            
            //Instantiate pickingCarts
            for (int i = 0; i < orders.Length; i++)
            {
                var position1 = firstPoint.transform.position;
                Vector3 position = new Vector3(position1.x + currentOffset, position1.y, position1.z);
                //GameObject box = Instantiate(boxPrefab, position, boxPrefab.transform.rotation, workstation.transform);
                GameObject box = Instantiate(pickingCartPrefab, position, pickingCartPrefab.transform.rotation,
                    workstation.transform);

                boxes[i] = box.GetComponent<PickingCart>();
                boxes[i].orderNumberSign.GetComponent<TextMeshPro>().SetText(orders[i].numberExtern.ToString());
                boxes[i].SetOrderManagerAndNumber(this, i, orders[i].numberExtern);
                if (barcodeSprites.Length - 1 >= i)
                {
                    boxes[i].Barcode.GetComponentInChildren<SpriteRenderer>().sprite = barcodeSprites[i];

                }
                else
                {
                    boxes[i].Barcode.GetComponentInChildren<SpriteRenderer>().sprite = barcodeSprites[0];
                }

                boxes[i].Barcode.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                PrepareOrders(i);

                orderComplete[i] = false;


                if (direction)
                    currentOffset += boxOffset;
                else
                    currentOffset -= boxOffset;
            }

            PreparePalletPlaces();
        }
        

        /// <summary>
        /// Big Item entered a pallet place
        /// TODO: Switch to new system, that the orderManager knows all needed pallet places and asks them, if the pallets are in
        /// </summary>
        /// <param name="bigItem"></param>
        /// <param name="value"></param>
        /// <param name="orderNumberIntern"></param>
        /// <param name="palletPlaceId"></param>
        public void UpdateBigItem(BigItem bigItem, bool value, int orderNumberIntern, int palletPlaceId)
        {

            //Debug.Log("UpdateBigItem "+bigItem.GetId()+", Value: "+value);

            foreach (var bigItemOrder in bigItemsinOrders)
            {
                if (bigItemOrder.palletPlaceID == palletPlaceId)
                {
                    bigItemOrder.isInPlace = value;

                }
            }

            if (checkOrdersAutomatically && CheckOrdersComplete())
                OrdersCompleted();
        }


        //returns if the big Items of the selected order are placed correctly
        public bool CheckBigItemsOfCurrentOrder()
        {
            foreach (var bigItem in bigItemsinOrders)
            {
                if (bigItem.orderNumberExtern == selectedOrderIdExtern)
                {
                    if (bigItem.isInPlace)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckIfBigItemsInOrderAreComplete(int numberIntern)
        {
            foreach (var bigItem in bigItemsinOrders)
            {
                if (bigItem.orderNumberIntern == numberIntern)
                {
                    if (!bigItem.isInPlace)
                        return false;
                }
            }

            return true;
        }

        private void OrdersCompleted()
        {
            Debug.Log("Orders completed!");
            successSound.Play();
        }


        private void OrdersNotCompleted()
        {
            failSound.Play();
        }

        
        /// <summary>
        /// Check for all orders, if small and big Items are inPLace
        /// </summary>
        /// <returns></returns>
        public bool CheckOrdersComplete()
        {
            foreach (var bigItem in bigItemsinOrders)
            {
                Debug.Log("Checking big Items");
                if (bigItem.isInPlace == false)
                    return false;
            }


            foreach (var cart in boxes)
            {

                if (!cart.AllItemsCorectInPickuingCart())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns all items needed for the selected oder. Returns null if no order selected
        /// </summary>
        public Dictionary<Item, int> ItemsNeededInCurrentOrder()
        {
            if (selectedOrderIdExtern != -1)
            {
                foreach (var cart in GetPickingCarts())
                {
                    if (cart.GetOrderNumberExtern() == selectedOrderIdExtern)
                    {
                        return cart.allNeededItems;
                    }
                }
            }

            return null;
        }

        public List<BigItem> GetBigItemsOfCurrentOrder()
        {
            List<BigItem> bigItems=new List<BigItem>();
            foreach (var bigItem in bigItemsinOrders)
            {
                if (bigItem.orderNumberExtern == selectedOrderIdExtern)
                {
                    bigItems.Add(bigItem.BigItemScript);
                }
            }

            return bigItems;
        }

        /// <summary>
        /// </summary>
        /// <returns>PickingCart of the current selected order</returns>
        public PickingCart GetPickingCartOfSelectedOrder()
        {
            if (selectedOrderIdExtern != -1)
            {
                foreach (var cart in GetPickingCarts())
                {
                    if (cart.GetOrderNumberExtern() == selectedOrderIdExtern)
                    {
                        return cart;
                    }
                }
            }

            return null;
        }
        
        

        [ContextMenu("Check if orders Complete")]
        public void CheckIfAllOrdersCompleted()
        {
            if (Time.time >= lastCheckButtonPress + buttonPressDelay)
            {
                lastCheckButtonPress = Time.time;
                if (CheckOrdersComplete())
                    OrdersCompleted();
                else
                    OrdersNotCompleted();
            }
            guidingController.UserEvent(new SubmitOrders(DateTime.Now, XesLifecycleTransition.complete));
        }

        /*
         * Compares two dictionaries, returns true, if they are equal 
         */
        private bool CompareOrders(Dictionary<string, int> should, Dictionary<string, int> have)
        {
            //if the key count is equal, it is sufficient to check only the keys in one dictionary
            foreach (var pair in have)
            {
               
            }
            if (should.Keys.Count != have.Keys.Count)
                return false;
            
            if (should.Keys.Count==0)
                throw new Exception("order must contain at least one item");

         
            foreach (var key in should.Keys)
            {
               try
                {
                    if (should[key] != have[key])
                        return false;
                }
                //dictionaries dont have the same keys, return false
                catch (KeyNotFoundException e)
                {
                    Debug.Log(e);
                    return false;
                }
                
            }

            return true;

        }
        
        //iterate through orders and count needed items
        private void PrepareOrders(int number)
        {
            
            Dictionary<Item, int> counterDict = new Dictionary<Item, int>();
            List<BigItemInOrder> bigItemsForScreen= new List<BigItemInOrder>();
            foreach (var article in orders[number].articleOrders)
            {
                
                if (!article.item.id.Contains("big"))
                {
                    
                    counterDict.Add(article.item, article.count);
                    
                }
                else
                {
                    //add bigOrders
                    bigItemsinOrders.Add(new BigItemInOrder(article.item.id,number,orders[number].numberExtern,(BigItem)article.item));
                    bigItemsForScreen.Add(new BigItemInOrder(article.item.id,number,orders[number].numberExtern,(BigItem)article.item));
                    
                }
                
            }
            
            boxes[number].SetAllNeededItems(counterDict, bigItemsForScreen);
            bigItemsForScreen.Clear();
           
        }

        /// <summary>
        /// Select pallet places to use in this run and distribute the information about the pallets which needs to be placed there
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void PreparePalletPlaces()
        {
            
            PalletGroundController[] palletGroundControllers = palletPlaces.GetComponentsInChildren<PalletGroundController>();
            //Debug.Log("Instantiating Pallets "+palletPlacePoints.Length+", "+bigItemsinOrders.Count);
            
            //add all palletplace Indices to list, to pick random pallet place for each bigItem without picking one place twice
            List<int> possiblePalletPlaceIndices= new List<int>();
            int counter = 0;
            foreach (var pgc in palletGroundControllers)
            {
                possiblePalletPlaceIndices.Add(counter);
                counter++;
            }
            if (bigItemsinOrders.Count <= palletGroundControllers.Length)
            {
                for (int i = 0; i < bigItemsinOrders.Count; i++)
                {
                    Debug.Log("Instantiate palletplacenr: "+i);
                    //PalletGroundController controller = Instantiate(palletPlacePrefab, palletPlacePoints[i].ReturnMyTransform())
                    //    .GetComponent<PalletGroundController>();

                    int palletPlaceIndex =
                        possiblePalletPlaceIndices[Random.Range(0, possiblePalletPlaceIndices.Count - 1)];
                    possiblePalletPlaceIndices.Remove(palletPlaceIndex);
                    palletGroundControllers[palletPlaceIndex].orderManager = this;
                    palletGroundControllers[palletPlaceIndex].orderNumberIntern = bigItemsinOrders[i].orderNumberIntern;
                    palletGroundControllers[palletPlaceIndex].palletPlaceID = palletPlaceIndex;
                    palletGroundControllers[palletPlaceIndex].itemId = bigItemsinOrders[i].id;
                    palletGroundControllers[palletPlaceIndex].orderNumberExtern = bigItemsinOrders[i].orderNumberExtern;
                    bigItemsinOrders[i].SetPalletPlaceId(palletPlaceIndex);
                }
                
            }
            else
            {
                throw new Exception("Too many big items ordered");
            }
            
        }

        

        public PickingCart[] GetPickingCarts()
        {
            return boxes;
        }
    }
}
