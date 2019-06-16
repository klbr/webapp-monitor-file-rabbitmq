using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VirusTotalServices;
using VirusTotalServices.Interface;
using VirusTotalServices.Models;
using WebAppDesafio.Controllers;
using WebAppDesafio.Models;
using Xunit;

namespace Desafio.Tests.Controllers
{
    public class ProcessControllerTest
    {
        private readonly IVirusTotalService virusTotalService;
        private readonly ProcessController controller;

        public ProcessControllerTest()
        {
            virusTotalService = Substitute.For<IVirusTotalService>();
            controller = new ProcessController(virusTotalService);
        }

        [Fact]
        public void ReceiveExeFileInputShouldBeSucess()
        {
            //arrange
            var filename = "app.exe";
            var resource = "testeRes";
            var controllerContext = CreateContextWithFile(filename);
            virusTotalService.ScanFile(filename, Arg.Any<byte[]>()).Returns(new ScanOutput { Resource = resource });
            controller.ControllerContext = controllerContext;

            //act
            var result = (OkObjectResult)controller.Index();
            var response = result.Value as ProcessModel;
            
            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(resource, response.Resource);
        }

        [Fact]
        public void ReceiveNoFileInputMustFail()
        {
            //arrange
            //act
            var result = (BadRequestObjectResult)controller.Index();

            //assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("File not found", result.Value);
        }

        [Fact]
        public void ReceiveTxtFileInvalidMustFail()
        {
            //arrange
            var controllerContext = CreateContextWithFile("file.txt");
            controller.ControllerContext = controllerContext;

            //act
            var result = (BadRequestObjectResult)controller.Index();

            //assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Unexpected extension", result.Value);
        }

        private static ControllerContext CreateContextWithFile(string fileName)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("")), 0, 0, fileName, fileName);
            context.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actionContext = new ActionContext(context, new RouteData(), new ControllerActionDescriptor());
            return new ControllerContext(actionContext); 
        }
    }
}
