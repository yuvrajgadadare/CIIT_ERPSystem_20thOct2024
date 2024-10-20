using ERPSystem_Models;
using ERPSystem_Services.Implementations;
using ERPSystem_Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddTransient<IEnquiryService, EnquiryService>();
builder.Services.AddTransient<IMasterService, MasterService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IExtraService, ExtraService>();
builder.Services.AddTransient<IBatchService, BatchService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<ITrainerService, TrainerService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
//builder.WebHost.ConfigureKestrel(options =>
//{
//    long mb = 1048576;
//    options.Limits.MaxRequestBodySize = 150 * mb;
//});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
    options.MultipartHeadersLengthLimit = int.MaxValue;

});
var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // This will handle exceptions and redirect to the specified error page.
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
//app.MapAreaControllerRoute(
//    areaName:"Counsellor",
//    name:"Counsellor",
//    pattern:"Counsellor/{controller=Dashboard}/{action=Index}/{id?}"
//    );
//app.MapControllerRoute(
//        name: "areaRoute",
//        pattern: "{area:exists}/{controller}/{action}/{id?}"
//    );

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Login}/{id?}");

app.Run();
