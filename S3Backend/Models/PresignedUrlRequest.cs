using System.Collections.Generic;

namespace S3Backend.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PresignedUrlRequest : Bucket
    {
        /// <summary>
        /// 
        /// </summary>
        public string UploadId { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public IList<int> PartNumbers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
    }
}
