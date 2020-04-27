namespace S3Backend.Models
{
    /// <summary>
    /// Bucket creation request model
    /// </summary>
    public class CreateBucketRequest
    {
        /// <summary>
        /// Gets or sets the BucketName
        /// </summary>
        public string BucketName { get; set; }
    }
}
