//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using System;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEngine;
using Valve.VR.InteractionSystem;



	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class ForkliftMovementController : MonoBehaviour
	{

		private Vector3 oldPosition;
		private Quaternion oldRotation;

		private float attachTime;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

        private Interactable interactable;

        public AudioSource runningAudio;
        public AudioSource impactAudio;
        

        //private Vector3 handStartPosition;

       
        public GameObject steeringPivot;
        public GameObject objectToMove;
        public GameObject steererPoint;
        public GameObject PlayerPoint;
        private GameObject HandStartingPoint;
      
        public float RotationRangeX;
        public float RotationRangeY;
        public float RotationRangeZ;

        public bool invertedFront;
        public bool invertedSide;
        
        public float speedFront;
        public float speedSide;

        public GameObject debugConsole;
        //public GameObject calculatedPos;
        //public GameObject facingPos;

        [HideInInspector]
        public Transform forklift;
        //private Vector3 setBackDirection;


        public float distanceToSetBack;
        public float distanceToSetBackPallet;
        public float settingBackDuration;
        private bool settingBack;
        private Vector3 settingBackStartPos;
        private Vector3 settingBackEndPos;

        private float settingBackStartTime;

        [HideInInspector]
        public bool inPallet;


        private float[] speedValues;

        private Hand handTouchedLast;
        private bool handIsAttached;

        //False if player is attached to forklift, true if not
        private bool transformParentRemoved;

        private GuidingController guidingController;
        //-------------------------------------------------
		void Awake()
		{
			var textMeshs = GetComponentsInChildren<TextMesh>();

			interactable = this.GetComponent<Interactable>();
			forklift = transform.parent.parent;
			HandStartingPoint = new GameObject();//
			HandStartingPoint.transform.parent = forklift;
			
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand is null && startingGrabType != GrabTypes.None)
            {
                // Save our position/rotation so that we can restore it when we detach
                oldPosition = transform.position;
                oldRotation = transform.rotation;

                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(interactable);

                // Attach this object to the hand
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
            else if (isGrabEnding)
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(interactable);

                // Restore position/rotation
                transform.position = steererPoint.transform.position;
                transform.rotation = forklift.rotation;
            }
		}


		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
		{
			
			handTouchedLast = hand;
			HandStartingPoint.transform.position=hand.transform.position;
			
			//there are two meshes for the steerer. one which is static and the other can be moved
			GetComponent<MeshRenderer>().enabled = false;
			objectToMove.GetComponent<MeshRenderer>().enabled = true;
			
			//attach the player to the forklift
			hand.transform.parent.parent.SetParent(PlayerPoint.transform);
			runningAudio.Play();
			transformParentRemoved = false;
			handIsAttached = true;

		}



		//-------------------------------------------------
		// Called when this GameObject is detached from the hand
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			runningAudio.Stop();
			objectToMove.transform.rotation = oldRotation;
			//deactivate the moving mesh and activate the static
			GetComponent<MeshRenderer>().enabled = true;
			objectToMove.GetComponent<MeshRenderer>().enabled = false;
			
			//detach the player from the forklift when the forklift is not moving anymore
			if (!settingBack)
			{
				hand.transform.parent.parent.SetParent(null);
				transformParentRemoved = true;
			}

			handIsAttached = false;

		}


		//-------------------------------------------------
		// Called every Update() while this GameObject is attached to the hand
		//-------------------------------------------------
		/// <summary>
		/// the player grabs the steerer. Calculate the hands current position relative to the position where the player starts grabbing.
		/// Rotate the steerer in the hands current direction relative to the start position.
		/// </summary>
		/// <param name="hand"></param>
		private void HandAttachedUpdate( Hand hand )
		{
			//calculate the hands relative position
			Vector3 relativePos = hand.transform.position-HandStartingPoint.transform.position;
			Vector3 tmp = (steeringPivot.transform.position + relativePos);
			
			
			//remove distance between the streering pivot point and the steerer's mesh pivot point
			Vector3 differenceSteeringPivotGameobject = steeringPivot.transform.position - objectToMove.transform.position;
			//calculate rotation for the steerer
			Quaternion rotation = Quaternion.LookRotation(tmp-objectToMove.transform.position-new Vector3(0,differenceSteeringPivotGameobject.y,0));

			
			Vector3 forkliftRotation = new Vector3(forklift.rotation.eulerAngles.x%360, forklift.rotation.eulerAngles.y%360,forklift.rotation.eulerAngles.z%360);
			
			Vector3 eulers = (rotation.eulerAngles) - forkliftRotation;
			
			eulers.y = (eulers.y+360) % 360;
			
			string eulersOld = eulers.ToString();
			
			//Ensure that the rotation is within the given bounds
			if (eulers.x > RotationRangeX && eulers.x <= 180)
			{
				eulers.x = RotationRangeX;
			}
			else if (eulers.x < 360 - RotationRangeX && eulers.x > 180)
			{
				eulers.x = 360-RotationRangeX;
			}

			if (eulers.y > RotationRangeY && eulers.y <= 180)
			{
				eulers.y = RotationRangeY;
			}
			else if (eulers.y < 360 - RotationRangeY && eulers.y > 180)
			{
				eulers.y = 360-RotationRangeY;
			}
			else if (eulers.y==360)
			{
				eulers.y = 0;
			}

			if (eulers.z > RotationRangeZ && eulers.z <= 180)
			{
				eulers.z = RotationRangeZ;
			}
			else if (eulers.z < 360 - RotationRangeZ && eulers.z > 180)
			{
				eulers.z = 360-RotationRangeZ;
			}
			
			objectToMove.transform.rotation = Quaternion.Euler(eulers+forklift.rotation.eulerAngles);
		
			Move(hand.transform.parent.parent,eulers);
		}
		

        private bool lastHovering = false;
        

        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                
                lastHovering = interactable.isHovering;
            }
            
            if(settingBack)
            {
	            PerformSetBack();
            }

        }

        private void Start()
        {
	        guidingController=FindObjectOfType<GuidingController>();
        }


        //-------------------------------------------------
		// Called when this attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusAcquired( Hand hand )
		{
		}


		//-------------------------------------------------
		// Called when another attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
		}

		/// <summary>
		/// Moves the Forklift according to the given angles of the steerer
		/// </summary>
		/// <param name="playerTransform"></param>
		/// <param name="eulerAngles"></param>
		private void Move(Transform playerTransform,Vector3 eulerAngles)
		{
			if (!settingBack)
			{
				speedValues = GetSpeed(eulerAngles);
				Vector3 newPosition;
				Vector3 newPositionPlayer;
				Vector3 positionUpdate = forklift.forward * (Time.deltaTime * speedFront * speedValues[0]);
				if (!invertedFront)
				{
					newPosition = forklift.position + positionUpdate;
					
				}
				else
				{
					newPosition = forklift.position - positionUpdate;
					
				}

				forklift.position = newPosition;

				Vector3 rotate;
				if(!invertedSide)
					rotate = new Vector3(0, Time.deltaTime * speedSide * speedValues[1], 0);
				else
					rotate = new Vector3(0, -1*Time.deltaTime * speedSide * speedValues[1], 0);
				
				forklift.Rotate(rotate);
			}
			



		}
		
		//0:front, 1:side
		//return: speed: -1=backwards /left, 1=forwards/right
		/// <summary>
		/// Calculates the speed and direction in which the forklift should move on the X and Z axis.
		/// </summary>
		/// <param name="eulerAngles"></param>
		/// <returns></returns>
		private float[] GetSpeed(Vector3 eulerAngles)
		{
			Vector3 rotation = objectToMove.transform.rotation.eulerAngles;
			float rotatioonOld = rotation.y;
			
			rotation.y = ((rotation.y-forklift.rotation.eulerAngles.y)+360)%360;
			
			
			
			float front;
			float side;
			if(rotation.x<RotationRangeX)
				front = Map(rotation.x,0, RotationRangeX, 0, 1);
			else if (rotation.x > 360 - RotationRangeX)
				front = Map(rotation.x, 360 - RotationRangeX, 360, -1, 0);
			else if (rotation.x >= RotationRangeX && rotation.x <= 180)
				front = 1;
			else if (rotation.x <= 360 - RotationRangeX && rotation.x > 180)
				front = -1;
			
			else
				front = 0;
			
			if(rotation.y<=RotationRangeY&&rotation.y>=0)
				side = Map(rotation.y,0, RotationRangeY, 0, 1);
			else if (rotation.y >= 360 - RotationRangeY&&rotation.y<360)
				side = Map(rotation.y, 360 - RotationRangeY, 360, -1, 0);
			else if (rotation.y > RotationRangeY && rotation.y <= 180)
				side = 1;
			else if (rotation.y < 360 - RotationRangeY && rotation.y > 180)
				side = -1;
			else
				side = 0;

			side = -side;
			//debugConsole.GetComponent<DebugConsole>().SetText("eulerAngles: " + eulerAngles+"\nrotationForklift: "+forklift.transform.rotation.eulerAngles.y+"\ncalculatedRotation: "+rotation.y+"\ncalculatedValue: "+side);
			
			//return new float[]{front,side};
			return new float[]{front,side};
		}
		
		/// <summary>
		/// Utility function to map a value from one range to another
		/// </summary>
		/// <param name="value"></param>
		/// <param name="from1"></param>
		/// <param name="to1"></param>
		/// <param name="from2"></param>
		/// <param name="to2"></param>
		/// <returns></returns>
		public float Map ( float value, float from1, float to1, float from2, float to2) {
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}


		/// <summary>
		/// Calculate the opposite of the direction in which the forklift hits an object. Initializes driving the forklift back
		/// </summary>
		/// <param name="isPallet"></param>
		public void SetBackStart(bool isPallet)
		{
			settingBack = true;
			settingBackStartTime = Time.time;
			settingBackStartPos = forklift.position;
			int direction;
			if (speedValues[0] < 0)
			{
				direction = -1;
			}
			else
			{
				direction = 1;
			}
			if(!isPallet)
				settingBackEndPos = settingBackStartPos - (-forklift.forward * (distanceToSetBack * direction));
			else
				settingBackEndPos = settingBackStartPos - (-forklift.forward * (distanceToSetBackPallet * direction));
			
			impactAudio.Play();
			string wallOrPallet;
			if (isPallet)
				wallOrPallet = "pallet";
			else
			{
				wallOrPallet = "wall";
			}
			guidingController.UserEvent(new ForkliftHitsObject(DateTime.Now, XesLifecycleTransition.complete));
			//logger.WriteEvent(new ForkliftHitsObject(forklift.position,DateTime.Now, XesLifecycleTransition.complete, new XesString(wallOrPallet)));
				
		}

		private void PerformSetBack()
		{

			float fracComplete = (Time.time - settingBackStartTime) / settingBackDuration;
			forklift.position = Vector3.Slerp(settingBackStartPos, settingBackEndPos, fracComplete);
			if (fracComplete >= 1)
			{
				settingBack = false;
				if(!transformParentRemoved&&!handIsAttached)
				{
					handTouchedLast.transform.parent.parent.SetParent(null);
					transformParentRemoved = true;}
			}


		}

		public void DebugPrint(string text)
		{
			if(debugConsole!=null)
				debugConsole.GetComponent<DebugConsole>().SetText(text);
		}
	}

