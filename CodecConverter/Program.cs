using System.Diagnostics;

string videoUrl = "http://124.29.204.5:6611/3/5?DownType=3&DevIDNO=TMK-807&FLENGTH=701333&FOFFSET=0&MTYPE=1&FPATH=E%3A%2FgStorage%2FRECORD_FILE%2F000001340093%2F2023-02-09%2F02_00_6505_03_00000001340093230209175416000500.mp4&SAVENAME=02_00_6505_03_00000001340093230209175416000500.mp4&jsession=7E9050C15DF69765B26CDBAA6BB093D1";
string videoFileName = "video.mp4";
string convertedVideoFileName = "converted_" + videoFileName;

try
{
    // Download the video
    using (var client = new HttpClient())
    {
        using (var response = await client.GetAsync(videoUrl, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
            {
                using (var streamToWriteTo = File.Create(videoFileName))
                {
                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                }
            }
        }
    }
    Console.WriteLine("Download complete.");
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while downloading the video: " + ex.Message);
    return;
}

try
{
    // Convert the video
    var ffmpegProcess = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-i {videoFileName} -c:v h264 -c:a aac {convertedVideoFileName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };
    ffmpegProcess.Start();
    Console.WriteLine(ffmpegProcess.StandardOutput.ReadToEnd());
    ffmpegProcess.WaitForExit();
    Console.WriteLine("Conversion complete.");
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while converting the video: " + ex.Message);
    return;
}


// using System.Diagnostics;

// string videoUrl = "http://124.29.204.5:6611/3/5?DownType=3&DevIDNO=TMK-807&FLENGTH=701333&FOFFSET=0&MTYPE=1&FPATH=E%3A%2FgStorage%2FRECORD_FILE%2F000001340093%2F2023-02-09%2F02_00_6505_03_00000001340093230209175416000500.mp4&SAVENAME=02_00_6505_03_00000001340093230209175416000500.mp4&jsession=7E9050C15DF69765B26CDBAA6BB093D1";
// string convertedVideoFileName = "converted_video.mp4";

// try
// {
//     // Download and convert the video
//     using (var client = new HttpClient())
//     {
//         using (var response = await client.GetAsync(videoUrl, HttpCompletionOption.ResponseHeadersRead))
//         {
//             response.EnsureSuccessStatusCode();

//             using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
//             {
//                 var ffmpegProcess = new Process
//                 {
//                     StartInfo = new ProcessStartInfo
//                     {
//                         FileName = "ffmpeg",
//                         Arguments = $"-i pipe:0 -c:v h264 -c:a aac {convertedVideoFileName}",
//                         RedirectStandardInput = true,
//                         RedirectStandardOutput = true,
//                         UseShellExecute = false,
//                         CreateNoWindow = true
//                     }
//                 };

//                 ffmpegProcess.Start();

//                 await streamToReadFrom.CopyToAsync(ffmpegProcess.StandardInput.BaseStream);

//                 Console.WriteLine(ffmpegProcess.StandardOutput.ReadToEnd());
//                 ffmpegProcess.WaitForExit();
//             }
//         }
//     }
//     Console.WriteLine("Conversion complete.");
// }
// catch (Exception ex)
// {
//     Console.WriteLine("An error occurred while converting the video: " + ex.Message);
//     return;
// }