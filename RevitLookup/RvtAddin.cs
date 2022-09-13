﻿/*
 * Created By WeiGan 2021.9.9
 * 1.Addin Class
 */

using System.IO;
using System.Reflection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using RevitLookupWpf.Commands;
using RevitLookupWpf.Helpers;

namespace RevitLookupWpf
{
    public class RvtAddin : IExternalApplication
    {
        #region Public Methods
        public Result OnStartup(UIControlledApplication application)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            RevitInfoManager.Version = GetRevitVersion(application.ControlledApplication);
            //InitUI(application);
            CreateRibbonPanel(application);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Failed;
        }
        #endregion

        #region Private Methods

        private RevitVersion GetRevitVersion(ControlledApplication controlledApplication)
        {
            return new RevitVersion(
                controlledApplication.VersionName,
                controlledApplication.VersionNumber,
                controlledApplication.VersionBuild,
                controlledApplication.SubVersionNumber);
        }

        //private void InitUI(UIControlledApplication application)
        //{
        //    string panelName = "RvtWpfLookup";

        //    //创建按钮
        //    var btns = CreatePushBtns(
        //        typeof(SnoopDBCommand),
        //        typeof(SnoopCurrentSelectionCommand),
        //        typeof(SnoopActiveDocCommand),
        //        typeof(SnoopExternalCommandData))
        //        .ToList();

        //    //创建Panel
        //    var ribbonPanel = application.CreateRibbonPanel(panelName);

        //    // Add the buttons to the panel
        //    btns.ForEach(btn => ribbonPanel.AddItem(btn));
        //}
        private static void CreateRibbonPanel(UIControlledApplication application)
        {
            var ribbonPanel = application.CreateRibbonPanel("Snoop");
            var pulldownButtonData = new PulldownButtonData("Options", "Revit Lookup");
            var pulldownButton = (PulldownButton)ribbonPanel.AddItem(pulldownButtonData);
            pulldownButton.Image = BitmapSourceConverter.ToImageSource(Resource.search, BitmapSourceConverter.ImageType.Small);
            pulldownButton.LargeImage = BitmapSourceConverter.ToImageSource(Resource.search, BitmapSourceConverter.ImageType.Large);
            AddPushButtonAva(pulldownButton, typeof(SnoopDBCommand), "Snoop DB...(Working In Process)");
            AddPushButton(pulldownButton, typeof(SnoopActiveDocCommand), "Snoop Active Document...");
            AddPushButton(pulldownButton, typeof(SnoopActiveViewCommand), "Snoop Active View...");
            AddPushButton(pulldownButton, typeof(SnoopCurrentSelectionCommand), "Snoop Current Selections...");
            AddPushButton(pulldownButton, typeof(SnoopPointsCommand), "Snoop Points...");
            AddPushButton(pulldownButton, typeof(SnoopFacesCommand), "Snoop Faces...");
            AddPushButton(pulldownButton, typeof(SnoopEdgesCommand), "Snoop Edges...");
            AddPushButton(pulldownButton, typeof(SnoopPointOnEleCommand), "Snoop Points On Elements...");
            AddPushButton(pulldownButton, typeof(SnoopGeometryCommand), "Snoop Geometry Element...");
            AddPushButton(pulldownButton, typeof(SnoopLinkedElementCommand), "Snoop Linked Element...");
            AddPushButton(pulldownButton, typeof(SnoopExternalCommandData), "Snoop ExternalCommandData...");
            AddPushButton(pulldownButton, typeof(SnoopSearchCommand), "Snoop Search Element...");
        }

        private static PushButton AddPushButton(PulldownButton pullDownButton, Type command, string buttonText)
        {
            var buttonData = new PushButtonData(command.FullName, buttonText, Assembly.GetAssembly(command).Location, command.FullName);
            return pullDownButton.AddPushButton(buttonData);
        }
        private static PushButton AddPushButtonAva(PulldownButton pullDownButton, Type command, string buttonText)
        {
            var buttonData = new PushButtonData(command.FullName, buttonText, Assembly.GetAssembly(command).Location,
                command.FullName);
            buttonData.AvailabilityClassName = typeof(SnoopDBCommandAvail).FullName;
            return pullDownButton.AddPushButton(buttonData);
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var dir = Path.GetDirectoryName(typeof(RvtAddin).Assembly.Location);
            var dllLocation = Path.Combine(dir, assemblyName.Name + ".dll");

            if (File.Exists(dllLocation))
            {
                return Assembly.LoadFrom(dllLocation);
            }

            return null;
        }
        #endregion
    }
}
