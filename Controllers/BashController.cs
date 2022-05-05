using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace shell_over_rest.Controllers;

[ApiController]
[Route("[controller]")]
public class BashController : Controller
{
    [HttpGet("[action]")]
    public IActionResult test()
    {
        return Ok("Hello World!");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> exec([FromBody] ExecFormBody body)
    {
        var escapedArgs = body.command.Replace("\"", "\\\"");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();

        var result = "";
        if (body.readOutput) result = await process.StandardOutput.ReadToEndAsync();
        if (body.waitForExit) await process.WaitForExitAsync();
        return Ok(result);
    }
    
    public class ExecFormBody
    {
        public string command { get; set; }
        public bool waitForExit { get; set; }
        public bool readOutput { get; set; }
    }
}