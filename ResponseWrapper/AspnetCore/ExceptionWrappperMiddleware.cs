using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ResponseWrapper.AspnetCore
{
    public class ExceptionWrappperMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionWrappperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var originalBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();
            //using var filteredResponse = new ResponseStreamWrapperx(context.Response.Body, context);
            using var memoryStreamResponse = new MemoryStream();
            //using var filteredResponse = new ResponseStreamWrapperx(context.Response.Body, context);
            var httpResponseFeature = context.Features.Get<IHttpResponseFeature>();
            try
            {
                // Use new IHttpResponseBodyFeature for abstractions of pilelines/streams etc.
                // For 3.x this works reliably while direct Response.Body was causing random HTTP failures
                //context.Features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(memoryStreamResponse));

                await _next(context);
                await memoryStreamResponse.CopyToAsync(originalBodyFeature.Stream);
                memoryStreamResponse.Seek(0, SeekOrigin.Begin);
                //problemResponse.Flush();                //problemResponse.Flush();
            }
            catch (System.Exception ex)
            {
                IError? error = ex as IError;

                if (error != null)
                {
                    var problem = error.GetProblemDetails(ex);
                    //using var problemResponse = new ResponseStreamWrapper(context.Response.Body, problem, context);

                    //context.Features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(problemResponse));
                    //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    //await problemResponse.FlushAsync();
                    //filteredResponse.Seek(0, SeekOrigin.Begin); ;
                    //JsonSerializer.SerializeToUtf8Bytes(problem);

                    //var problemByte = JsonSerializer.Serialize(problem);
                    //var stringContent = new StringContent(problemByte, Encoding.UTF8, "application/json");

                    //var problemByte = Encoding.UTF8.GetBytes("Problem ttttttttttttttttttttttttt");
                    //httpResponseFeature.StatusCode = StatusCodes.Status500InternalServerError;

                    memoryStreamResponse.Seek(0, SeekOrigin.Begin);

                    await JsonSerializer.SerializeAsync(memoryStreamResponse, problem);

                    //var problemByte = JsonSerializer.SerializeToUtf8Bytes(problem);

                    memoryStreamResponse.Seek(0, SeekOrigin.Begin);

                    //await originalBodyFeature.Stream.WriteAsync(problemByte, 0, problemByte.Length);
                    //httpResponseFeature.Headers.Add("content/type","application/json");
                    //await stringContent.CopyToAsync(originalBodyFeature.Stream);
                    //originalBodyFeature.Stream.Seek(0, SeekOrigin.Begin);

                    //httpResponseFeature.StatusCode = StatusCodes.Status500InternalServerError;
                    //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await memoryStreamResponse.CopyToAsync(originalBodyFeature.Stream);
                    memoryStreamResponse.Seek(0, SeekOrigin.Begin);
                }
            }
            finally
            {
                context.Features.Set(originalBodyFeature);
                context.Features.Set(httpResponseFeature);
            }
        }
    }

    public class ResponseStreamWrapperx : Stream
    {
        private Stream _baseStream;
        private readonly ProblemDetails problem;
        private HttpContext _context;

        public ResponseStreamWrapperx(Stream baseStream, ProblemDetails problem, HttpContext context)
        {
            _baseStream = baseStream;
            this.problem = problem;
            this._context = context;
            CanWrite = true;
        }
        public override bool CanRead { get; }

        public override bool CanSeek { get; }

        public override bool CanWrite { get; }

        public override long Length { get; }

        public override long Position { get; set; }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            _context.Response.Headers.ContentLength = null;
            return _baseStream.FlushAsync(cancellationToken);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override async void Write(byte[] buffer, int offset, int count)
        {
            var problemByte = Encoding.UTF8.GetBytes("Problem ttttttttttttttttttttttttt");

            await _baseStream.WriteAsync(problemByte, 0, problemByte.Length);
        }

        protected override void Dispose(bool disposing)
        {
            //_baseStream?.Dispose();
            _baseStream = null;
            _context = null;

            base.Dispose(disposing);
        }
    }
}
