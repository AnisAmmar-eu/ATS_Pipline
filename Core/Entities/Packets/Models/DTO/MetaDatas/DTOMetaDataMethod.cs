using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Mapster;

namespace Core.Entities.Packets.Models.DTO.MetaDatas;

public partial class DTOMetaData
{
	public DTOMetaData()
	{
		Type = PacketTypes.MetaData;
	}

	public override MetaData ToModel()
	{
		MetaData metaData = this.Adapt<MetaData>();
		Type = PacketTypes.MetaData;
		return metaData;
	}
}