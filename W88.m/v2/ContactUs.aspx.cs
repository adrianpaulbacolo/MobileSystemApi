﻿using System;
using Helpers;

public partial class v2_ContactUs : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", Pages.Dashboard);
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}