<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>


<div style="height:200px; display:block;">

<b>Tab2</b>
<br /> <br />
This tab has been loaded statically using the Html.RenderPartail mechanism:
<br />

.AddTabItem(new TabItem()<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <b><i>.SetContent("Tab2")</i></b>
<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; .SetTitle("PartialView"))

</div>