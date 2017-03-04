<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div style="height:200px; display:block;">

<b>Tab1</b>
<br /> <br />
This tab has been loaded using an ajax request to the action Tab1:
<br />

.AddTabItem(new TabItem()<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <b><i>.SetAction("Tab1")</i></b>
<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; .SetTitle("RemoteTab"))



</div>