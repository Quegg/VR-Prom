using System;
using System.Collections.Generic;
using Orders;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Stock
{
    public class Stock : MonoBehaviour
    {

        //bock prefab, racks object
        [Header("Small Items")]
        public GameObject racks;
        public GameObject boxPrefab;
        public StockItem[] stockItems;

        [Header("Big Items")] public GameObject palletSpawns;
        public GameObject undefinedPrefab;
        public StockItem[]bigItems;
		
        private Dictionary<string, List<OutlineController>> smallItemOutlines;
        private Dictionary<string, List<OutlineController>> bigItemOutlines;

        public Dictionary<string, List<OutlineController>> BigItemOutlines => bigItemOutlines;

        public Dictionary<string, List<OutlineController>> SmallItemOutlines => smallItemOutlines;
        public OrderManager orderManager;

        public StockItem[] GetItems()
        {
            return stockItems;
        }

        private void Start()
        {
            orderManager = FindObjectOfType<OrderManager>();
            GenerateItems();
            GenerateBigItems();
			
        }

        /// <summary>
        /// Pick a rack for every small item, and fill it
        /// </summary>
        private void GenerateItems()
        {
			
            List<RackPlatePsoitionRoation> positionAndRotation = racks.GetComponent<RackManager>().GetRackPlateStartPoints();
            foreach (var item in stockItems)
            {
                //Select Random Rack plate
                RackPlatePsoitionRoation plateForCurrentItem =
                    positionAndRotation[Random.Range(0, positionAndRotation.Count-1)];
                positionAndRotation.Remove(plateForCurrentItem);
				
                FillRack(item, plateForCurrentItem);
            }

            SmallItem boxScript = boxPrefab.GetComponent<SmallItem>();
            StockItem boxItem = new StockItem();
            boxItem.prefab = boxPrefab;
            boxItem.amount = boxScript.GetCountX() * boxScript.GetCountY() * boxScript.GetCountZ();

            foreach (var pos in positionAndRotation)
            {
                FillRack(boxItem,pos);
            }
        }

        /// <summary>
        /// Spawns the items on the designated rack plate
        /// </summary>
        /// <param name="item"></param>
        /// <param name="plateForCurrentItem"></param>
        private void FillRack(StockItem item, RackPlatePsoitionRoation plateForCurrentItem)
        {
            //all important values are stored in the small item script attached to the prefab
            SmallItem smallItemScript = item.prefab.GetComponent<SmallItem>();
            float offsetX = smallItemScript.GetOffsetX();
            float offsetY = smallItemScript.GetOffsetY();
            float offsetZ = smallItemScript.GetOffsetZ();

            int countXMax = smallItemScript.GetCountX();
            int countYMax = smallItemScript.GetCountY();
            int countZMax = smallItemScript.GetCountZ();
				
            // true, if Horizontal =>  prefab does not need to be turned, xoffset of the prefab is applied in x direction
            //false if Vertical => prefab needs to be turned 90°, xoffset of the prefab is applied in direction
            bool direction;

            //if(rack is horizontal)
            //Debug.Log(plateForCurrentItem.GetRoation());
            Quaternion testNull = Quaternion.Euler(0,0,0);
            Quaternion test180 = Quaternion.Euler(0, 180, 0);
            if (plateForCurrentItem.GetRoation() ==testNull || plateForCurrentItem.GetRoation() == test180)
            {
                //Debug.Log("TRUE");
                direction = true;
            }
            else
            {
                //direction = true;
                direction = false;
                //Debug.Log("False");
            }
				
				
            int counterX = 0;
            int counterY = 0;
            int counterZ = 0;
				
				
            Vector3 positionOriginal = plateForCurrentItem.GetPosition();
				
            if(direction)
            {
                positionOriginal = new Vector3(positionOriginal.x+offsetX/2, positionOriginal.y, positionOriginal.z+offsetZ/2);
            }
            else
            {
                positionOriginal = new Vector3(positionOriginal.x+offsetZ/2, positionOriginal.y, positionOriginal.z-offsetX/2);
                //positionOriginal = new Vector3(positionOriginal.x, positionOriginal.y, positionOriginal.z);
            }
				
            Vector3 position = positionOriginal;
            Quaternion rotation;


            List<OutlineController> _smallItemOutlines= new List<OutlineController>();
				
            for (int i = 0; i < item.amount; i++)
            {
                rotation = item.prefab.transform.rotation;
                //Istantiate
                if (direction)
                    rotation=Quaternion.Euler(rotation.eulerAngles.x,(rotation.eulerAngles.y + 90) % 360, rotation.eulerAngles.z);
                var newItem = Instantiate(item.prefab, position, rotation).GetComponent<OutlineController>();
                ItemMoveHelper helper = newItem.GetComponent<ItemMoveHelper>();
                helper.orderManager = orderManager;
                _smallItemOutlines.Add(newItem);
                
                if (counterX < countXMax-1)
                {
						
                    counterX++;
                    if (direction)
                        position.x += offsetX;
                    else
                        position.z -= offsetX;
                }
                else if (counterZ< countZMax-1)
                {
                    if (direction)
                        position.x = positionOriginal.x;
                    else
                        position.z = positionOriginal.z;
                    counterX = 0;
                    counterZ++;
                    if (direction)
                        position.z += offsetZ;
                    else
                        position.x += offsetZ;
                }
                else if (counterY<countYMax-1)
                {
                    position.x = positionOriginal.x;
                    position.z = positionOriginal.z;
                    counterX = 0;
                    counterZ = 0;
                    counterY++;
                    position.y += offsetY;
                }
                
            }
            plateForCurrentItem.SetName(smallItemScript.GetName());
            if (smallItemScript.id != "box")
            {
                if (smallItemOutlines is null)
                {
                    smallItemOutlines = new Dictionary<string, List<OutlineController>>();
                }

                smallItemOutlines.Add(smallItemScript.id, _smallItemOutlines);
            }
        }

        //spawn the pallets
        private void GenerateBigItems()
        {
            ReturnTransform[] palletSpawnPoints = palletSpawns.GetComponentsInChildren<ReturnTransform>();
            List<Transform> spawnPointTransforms= new List<Transform>();
            foreach (var sp in palletSpawnPoints)
            {
                spawnPointTransforms.Add(sp.ReturnMyTransform());
            }

			
            foreach (var bigItem in bigItems)
            {
                List<OutlineController> _bigOutlineControllers= new List<OutlineController>();
                for (int i = 0; i < bigItem.amount; i++)
                {
                    Transform currentPoint = spawnPointTransforms[Random.Range(0, spawnPointTransforms.Count - 1)];
                    _bigOutlineControllers.Add(Instantiate(bigItem.prefab, currentPoint).GetComponentInChildren<Barcode>().gameObject.GetComponentInChildren<OutlineController>());
                    spawnPointTransforms.Remove(currentPoint);
                }

                if (bigItemOutlines is null)
                {
                    bigItemOutlines = new Dictionary<string, List<OutlineController>>();
                }
                bigItemOutlines.Add(bigItem.prefab.GetComponentInChildren<BigItem>().id,_bigOutlineControllers);

            }

            foreach (var spt in spawnPointTransforms)
            {
                Instantiate(undefinedPrefab, spt);
            }
        }
    }
	
}