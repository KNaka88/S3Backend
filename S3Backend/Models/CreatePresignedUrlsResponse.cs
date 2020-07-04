namespace S3Backend.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatePresignedUrlsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PresignedUrl { get; set; }
    }
}
