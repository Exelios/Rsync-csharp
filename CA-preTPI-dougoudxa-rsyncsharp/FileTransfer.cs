/// ETML
/// Author: VinceMonkeyz
/// Date:   18.04.2016
/// 
/// Modified by: Xavier Dougoud
/// 
/// Based on the source code from http://codes-sources.commentcamarche.net/source/53449-transfert-de-fichier

using System;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Class used to simplify the implementation of the transfers.
    /// </summary>
    [Serializable]
    class FileTransfer
    {
        public String name;
        public Int64 size;
        public String hash;

        public FileTransfer(String name, Int64 size, String hash)
        {
            this.name = name;
            this.size = size;
            this.hash = hash;
        }
    }
}
