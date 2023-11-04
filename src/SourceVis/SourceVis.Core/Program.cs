// See https://aka.ms/new-console-template for more information

using SourceVisCore.AST;

var sln = await Parser.Parse("../../../../ShadowEngine.Gui.sln");

sln.Print();