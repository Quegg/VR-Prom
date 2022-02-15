using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Orders
{
	public class Order : MonoBehaviour
	{

		[FormerlySerializedAs("number")] public int numberExtern;
		public ArticleOrder[] articleOrders;
		public bool isCompleted = false;

		public ArticleOrder[] GetOrders()
		{
			return articleOrders;
		}
		
		
	}
}