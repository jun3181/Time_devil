import React from 'react';
import logo from './logo.svg';



export default function DownloadPage(){
  return(
    <div>
      <img src = {logo} style = {{top: "50px"}}></img>
      <h1>게임설명</h1>
      <button>다운로드</button>
    </div>
  )
}