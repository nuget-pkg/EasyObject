{config_load file="test.conf" section="setup"}
{include file="header.tpl" title=playlist}

{$playlistId}
<br>
<table>
{section name=index loop=$json}
    <tr>
      <td>
        <iframe width="560" height="315" src="https://www.youtube.com/embed/{$json[index].videoId}\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>
      </td>
      <td>
        <a href="https://www.youtube.com/watch?v={$json[index].videoId}" target="_blank">
        title: {$json[index].title}
        </a>
      </td>
    </tr>
{/section}
</table>


{include file="footer.tpl"}
