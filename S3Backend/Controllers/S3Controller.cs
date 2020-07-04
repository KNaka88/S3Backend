using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using S3Backend.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3Backend.Controllers
{
    /// <summary>
    /// API Controller that is responsible for handling S3 work
    /// </summary>
    [ApiController]
    [Route("api/s3")]
    public class S3Controller
    {
        private readonly ILogger<S3Controller> _logger;
        private readonly IAmazonS3 _s3;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger of type <see cref="ILogger"/></param>
        /// <param name="s3">S3 Client of <see cref="IAmazonS3"/></param>
        public S3Controller(ILogger<S3Controller> logger, IAmazonS3 s3)
        {
            _logger = logger;
            _s3 = s3;
        }

        /// <summary>
        /// Returns all buckets from S3.
        /// </summary>
        /// <returns>List of <see cref="S3Bucket"/></returns>
        [HttpGet("bucket/all")]
        public async Task<IActionResult> GetBuckets()
        {
            var bucketsResponse = await _s3.ListBucketsAsync();
            return new OkObjectResult(bucketsResponse.Buckets);
        }

        /// <summary>
        /// Returns all S3 Objects from S3.
        /// </summary>
        /// <returns>List of <see cref="S3Object"/></returns>
        [HttpPost("files/all")]
        public async Task<IActionResult> ListFiles([FromBody] Bucket bucket)
        {
            var response = await _s3.ListObjectsAsync(bucket.BucketName);
            return new OkObjectResult(response.S3Objects);
        }


        /// <summary>
        /// Create a new S3 bucket.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Bucket name</returns>
        [HttpPost("create/bucket")]
        public async Task<IActionResult> CreateBucket([FromBody] CreateBucketRequest request)
        {
            await _s3.PutBucketAsync(request.BucketName);

            var corsConfigurationRequest = new PutCORSConfigurationRequest
            {
                BucketName = request.BucketName,
                Configuration = new CORSConfiguration
                {
                    Rules = new List<CORSRule>
                    {
                        new CORSRule
                        {
                            AllowedOrigins = new List<string> { "https://localhost:3000" },
                            AllowedMethods = new List<string> { "PUT", "POST", "DELETE" },
                            AllowedHeaders = new List<string> { "*" },
                            ExposeHeaders = new List<string> { "ETag" }
                        },
                        new CORSRule
                        {
                            AllowedOrigins = new List<string> { "*" },
                            AllowedMethods = new List<string> { "GET" },
                        }
                    }
                }
            };
            await _s3.PutCORSConfigurationAsync(corsConfigurationRequest);

            return new OkObjectResult(request.BucketName);
        }

        /// <summary>
        /// Start Multipart Upload
        /// </summary>
        /// <returns>UploadId</returns>
        [HttpPost("Start_MultiPart")]
        public async Task<IActionResult> StartMultipartUpload([FromBody] Bucket request)
        {
            var uploadRequest = new InitiateMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = request.Key,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
            };
            var response = await _s3.InitiateMultipartUploadAsync(uploadRequest);

            return new OkObjectResult(new StartMultiPartUploadResponse { UploadId = response.UploadId });
        }

        /// <summary>
        /// Create PresignedUrl for each multi part upload
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create_PresignedUrl")]
        public IActionResult CreatePresignedUrl([FromBody] PresignedUrlRequest request)
        {

            var requests = request.PartNumbers.Select(partNumber => new GetPreSignedUrlRequest
            {
                BucketName = request.BucketName,
                Key = request.Key,
                Verb = HttpVerb.PUT,
                UploadId = request.UploadId,
                PartNumber = partNumber,
                ContentType = request.ContentType,
                Expires = DateTime.UtcNow.AddMinutes(30),
            });

            var responses = new BlockingCollection<CreatePresignedUrlsResponse>();

            Parallel.ForEach(requests, (request) =>
            {
                responses.Add(new CreatePresignedUrlsResponse
                {
                    PartNumber = request.PartNumber,
                    PresignedUrl = _s3.GetPreSignedURL(request)
                });
            });
            
            return new OkObjectResult(responses.OrderBy(x => x.PartNumber));           
        }

        /// <summary>
        /// Complete Multi part upload
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("Complete_MultiPartUpload")]
        public async Task<IActionResult> CompleteMultiPartUpload([FromBody] CompleteMultipartUploadRequest request)
        {
                await _s3.CompleteMultipartUploadAsync(request);
                return new OkResult();
        }
    }
}
