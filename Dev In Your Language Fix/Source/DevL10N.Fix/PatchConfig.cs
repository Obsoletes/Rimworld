using System.Collections.Generic;
using System.Xml.Serialization;

namespace DevelopMode
{
	[XmlRoot("Config")]
	public class PatchConfig
	{
		[XmlElement("Node")]
		public List<PatchNode> Nodes { get; set; }
	}
}
