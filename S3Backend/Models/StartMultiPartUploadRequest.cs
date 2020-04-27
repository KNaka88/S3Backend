namespace S3Backend.Models
{
    /// <summary>
    /// Bucket object model
    /// </summary>
    public class Bucket
    {
        /// <summary>
        /// Gets or sets the Bucket name
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public string Key { get; set; }
    }
}
