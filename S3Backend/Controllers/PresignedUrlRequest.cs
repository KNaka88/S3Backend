namespace S3Backend.Controllers
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
