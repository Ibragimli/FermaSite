﻿using Ferma.Service.CustomExceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ServiceExtentions
{
    public static class ExceptionHandlerExtention
    {
        public static void AddExceptionHandlerService(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var code = 500;
                    string message = "Inter Server Error. Please Try Again Later!";

                    if (contextFeature != null)
                    {
                        message = contextFeature.Error.Message;

                        if (contextFeature.Error is ItemNotFoundException)
                            code = 404;
                        if (contextFeature.Error is ImageFormatException)
                            code = 400;
                        if (contextFeature.Error is ImageNullException)
                            code = 404;
                        if (contextFeature.Error is AuthenticationCodeException)
                            code = 400;
                        if (contextFeature.Error is CookieNotActiveException)
                            code = 404;
                        if (contextFeature.Error is ItemNullException)
                            code = 404;
                        if (contextFeature.Error is ItemFormatException)
                            code = 400;
                        if (contextFeature.Error is PaymentValueException)
                            code = 400;
                        if (contextFeature.Error is NotFoundException)
                            code = 404;
                        if (contextFeature.Error is ItemAlreadyException)
                            code = 404;
                        if (contextFeature.Error is ValueAlreadyExpception)
                            code = 404;
                        if (contextFeature.Error is ImageCountException)
                            code = 400;
                        if (contextFeature.Error is UserNotFoundException)
                            code = 404;
                        if (contextFeature.Error is ExpirationDateException)
                            code = 400;
                        if (contextFeature.Error is ItemUseException)
                            code = 500;
                        if (contextFeature.Error is SmsSenderException)
                            code = 400;
                        if (contextFeature.Error is RareLimitException)
                            code = 429;
                        if (contextFeature.Error is ValueFormatExpception)
                            code = 400;
                        if (contextFeature.Error is UserPasswordResetException)
                            code = 400;
                    }

                    context.Response.StatusCode = code;

                    var errprJsonStr = JsonConvert.SerializeObject(new { code = code, message = message });

                    await context.Response.WriteAsync(errprJsonStr);
                });

            });

        }
    }
}
