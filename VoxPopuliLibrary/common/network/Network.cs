/**
 * Network shared protocol
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
namespace VoxPopuliLibrary.common.network
{
    public enum NetworkProtocol : ushort
    {
        ServerVersionSend =1,
        ChunkDemand = 30,
        ChunkData = 31,
        ChunkOneBlockChange = 32,
        ChunkAllChunkChange = 33,
        ChunkOneBlockChangeDemand = 34,
        ChunkMultipleBlockChangeDemand = 35,
        PlayerSpawnToClient = 60,
        PlayerSendControl = 61,
        PlayerPosition = 62,
        PlayerDeco = 63,
        PlayerLocal = 64,
        PlayerClientSendPos = 65,
    }
}
