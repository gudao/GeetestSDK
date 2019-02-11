#pragma checksum "D:\git\GeetestSDK\GeetestSDK\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d8e88ae7c34ab46bb27067ea8a9638cf62d16206"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\git\GeetestSDK\GeetestSDK\Views\_ViewImports.cshtml"
using GeetestSDK;

#line default
#line hidden
#line 2 "D:\git\GeetestSDK\GeetestSDK\Views\_ViewImports.cshtml"
using GeetestSDK.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d8e88ae7c34ab46bb27067ea8a9638cf62d16206", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cd649138f41d13bb55656826f6938a4598a92656", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\git\GeetestSDK\GeetestSDK\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(45, 4459, true);
            WriteLiteral(@"<script src=""http://code.jquery.com/jquery-1.12.3.min.js""></script>
<script src=""js/gt.js""></script>
<div>
    <div id=""captchaBox""></div>
    <div id=""captcha"">
        <div id=""text"">
            行为验证™ 安全组件加载中
        </div>
        <div id=""wait"" class=""show"">
            <div class=""loading"">
                <div class=""loading-dot""></div>
                <div class=""loading-dot""></div>
                <div class=""loading-dot""></div>
                <div class=""loading-dot""></div>
            </div>
        </div>
    </div>
</div>

<script>

    //$().ready(function () {

    //    $.ajax({
    //        url: ""/Home/getCaptcha?t="" + (new Date()).getTime(),
    //        type: ""get"",
    //        dataType: ""json"",
    //        success: function (data) {
    //            initGeetest({
    //                gt: data.gt,
    //                challenge: data.challenge,
    //                product: ""float"",
    //                offline: !data.success,
    //             ");
            WriteLiteral(@"   new_captcha: data.new_captcha
    //            }, function (captchaObj) {
    //                captchaObj.appendTo(""#captchaBox"");
                    
    //                captchaObj.onReady(function () {
    //                    //your code
    //                }).onSuccess(function () {
    //                    var result = captchaObj.getValidate();
                        
    //                    $.ajax({
    //                            url: '/Home/Index',
    //                            type: 'post',
    //                        data: {
    //                            challenge: result.geetest_challenge,
    //                            validate: result.geetest_validate,
    //                            seccode: result.geetest_seccode
    //                        },
    //                        success: function (data) {
    //                            console.log('server chenggong'+data.result);
    //                        }
    //                        });");
            WriteLiteral(@"
    //                    console.log('html chenggong')
    //                }).onError(function () {
    //                    //your code
    //                    console.log('cuow')
    //                })
    //            });
    //        }
    //    });
      
    //})




</script>
<script>


    var handler = function (captchaObj) {
        captchaObj.appendTo('#captcha');
        captchaObj.onReady(function () {
            $(""#wait"").hide();
        });
        $('#btn').click(function () {
            var result = captchaObj.getValidate();
            if (!result) {
                return alert('请完成验证');
            }
            $.ajax({
                url: '/Home/Index',
                type: 'POST',
                dataType: 'json',
                data: {
                    username: $('#username2').val(),
                    password: $('#password2').val(),
                    challenge: result.geetest_challenge,
                    validate: result.");
            WriteLiteral(@"geetest_validate,
                    seccode: result.geetest_seccode
                },
                success: function (data) {
                    if (data.status === 'success') {
                        alert('登录成功');
                    } else if (data.status === 'fail') {
                        alert('登录失败，请完成验证');
                        captchaObj.reset();
                    }
                }
            });
        })
        // 更多前端接口说明请参见：http://docs.geetest.com/install/client/web-front/
    };

    $.ajax({
        url: ""/Home/getCaptcha?t="" + (new Date()).getTime(), // 加随机数防止缓存
        type: ""get"",
        dataType: ""json"",
        success: function (data) {
            $('#text').hide();
            $('#wait').show();
            // 调用 initGeetest 进行初始化
            // 参数1：配置参数
            // 参数2：回调，回调的第一个参数验证码对象，之后可以使用它调用相应的接口
            initGeetest({
                // 以下 4 个配置参数为必须，不能缺少
                gt: data.gt,
                challenge: data.challenge,
            WriteLiteral(@"
                offline: !data.success, // 表示用户后台检测极验服务器是否宕机
                new_captcha: data.new_captcha, // 用于宕机时表示是新验证码的宕机

                product: ""float"", // 产品形式，包括：float，popup
                width: ""300px""

                // 更多前端配置参数说明请参见：http://docs.geetest.com/install/client/web-front/
            }, handler);
        }
    });
</script>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591