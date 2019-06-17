using Desafio.Domain;
using Desafio.Services.Interfaces;
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
using WebAppDesafio.Controllers;
using WebAppDesafio.Models;
using Xunit;

namespace Desafio.Tests.Controllers
{
    public class ProcessControllerTest
    {
        private readonly IReportService scannedFileService;
        private readonly ProcessController controller;

        public ProcessControllerTest()
        {            
            scannedFileService = Substitute.For<IReportService>();
            controller = new ProcessController(scannedFileService);
        }

        [Fact]
        public void ReceiveExeFileInputShouldBeSucess()
        {
            //arrange
            var filename = "app.exe";
            var controllerContext = CreateContextWithFile(filename);
            controller.ControllerContext = controllerContext;

            //act
            var result = (RedirectToActionResult)controller.Index();
            
            //assert
            Assert.Equal("Home", result.ControllerName);
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
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(".")), 0, 1, fileName, fileName);
            context.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actionContext = new ActionContext(context, new RouteData(), new ControllerActionDescriptor());
            return new ControllerContext(actionContext); 
        }
    }
}
