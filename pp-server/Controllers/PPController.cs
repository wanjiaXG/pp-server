﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace pp_server.Controllers
{
    [ApiController]
    public class PPController : ControllerBase
    {
        private readonly string _osuFilePath;

        public PPController(IConfiguration configuration)
        {
            _osuFilePath = configuration.GetValue<string>("OsuFilePath");
            DirectoryInfo info = new DirectoryInfo(_osuFilePath);
            if (!info.Exists)
            {
                info.Create();
            }
        }

        [HttpGet("/")]
        public string Index()
        {
            return "osu beatmap pp calculator";
        }

        [HttpGet("/pp")]
        public string GetPP(int bid, int mode, string mods, double acc, int miss, int combo, int good, int meh, int score, double percentCombo) 
        {
            return PPHelper.GetPP(_osuFilePath, bid, mode, mods, acc, miss, combo, good, meh, score, percentCombo);
        }
    }
}
