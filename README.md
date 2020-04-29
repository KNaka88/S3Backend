# S3 Backend

This repo shows an example of using presigned url with multi part upload.


### How to start?

1. Clone the repo
2. Go to the root folder
3. run `docker-compose up -d`
4. Open https://localhost:44362/swagger/index.html
5. You should be able to see the Swagger UI


### How to use my AWS Account instead of Localstack?

1. Create `docker-compose.override.yml` on root folder.
2. Add followings

```
version: '3.4'
services:
  s3backend:
    environment:
      - USE_LOCALSTACK=false
      - AWS_ACCESS_KEY=YOUR_ACCESS_KEY
      - AWS_SECRET_KEY=YOUR_SECRET_KEY
```

It is not recommended to use credentials keys for deployment.
For the production environment, consider using IAM Role


### How to implement frontend?

Coming soon...


#### Known Issues

* Localstack throws error when Server Side Encryption is added.
* See S3Controller.cs StartMultipartUpload
```
        public async Task<IActionResult> StartMultipartUpload([FromBody] Bucket request)
        {
            var uploadRequest = new InitiateMultipartUploadRequest
            {
                BucketName = request.BucketName,
                Key = request.Key,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256, // remove this line if you run with localstack
            };
            var response = await _s3.InitiateMultipartUploadAsync(uploadRequest);
            return new OkObjectResult(response.UploadId);
        }
```