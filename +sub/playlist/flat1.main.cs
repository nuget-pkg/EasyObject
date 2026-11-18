//css_nuget EasyObject
//css_nuget YoutubeExplode
//css_nuget YoutubeExplode.Converter
using System;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using static Global.EasyObject;

string playlistPath = @"G:\マイドライブ\.gas-youtube\.flat\_flat_playlist.json";
var playlist = FromFile(playlistPath);
playlist.Dump();
