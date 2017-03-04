<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div style="height:200px; display:block;">

<b>Accordion1</b>
<br /> <br />
This accordion item has been loaded using an ajax request to the action Accordion1:
<br />

.AddAccordionItem(new AccordionItem()<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <b><i>.SetContent(ContentType.RemoteAction, "Accordion1")</i></b>
<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; .SetTitle("Remote Action"))



</div>