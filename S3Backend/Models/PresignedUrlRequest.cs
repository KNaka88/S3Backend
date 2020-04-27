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
        public int PartNumber { get; set; }
    }
}
