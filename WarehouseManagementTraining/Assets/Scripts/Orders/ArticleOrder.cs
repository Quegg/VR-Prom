using System;

namespace Orders
{
	[Serializable]
	public class ArticleOrder
	{

		//public int orderNr;
		public Item item;
		public int count;
	}
}