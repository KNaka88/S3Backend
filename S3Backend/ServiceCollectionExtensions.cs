using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace S3Backend
{
    /// <summary>
    /// Extended method to initialize Service Collections
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure Local Stack for local development purpose.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public static void ConfigureLocalStack(this IServiceCollection services)
        {
            var awsCredentials = new BasicAWSCredentials("access_key", "secret_key");
            var amazonS3Config = new AmazonS3Config
            {
                ServiceURL = "http://localstack:4572",
                UseHttp = true,
                DisableLogging = false,
                ForcePathStyle = true
            };
            services.AddSingleton<IAmazonS3, AmazonS3Client>(s => new AmazonS3Client(awsCredentials, amazonS3Config));
        }

        /// <summary>
        /// Configure AWS Services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public static void ConfigureAWS(this IServiceCollection services)
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
            var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-west-2";
            var amazonS3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion)
            };

            if (accessKey != null && secretKey != null)
            {
                var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
                services.AddSingleton<IAmazonS3, AmazonS3Client>(s => new AmazonS3Client(awsCredentials, amazonS3Config));
            }
            else
            {
                services.AddSingleton<IAmazonS3, AmazonS3Client>(s => new AmazonS3Client(amazonS3Config));
            }
        }
    }
}
