import React from 'react';
import {useState} from 'react';
import logo from './logo.svg';
import './Homepage.css';
function Comment(){
  let [comments, setComments] = useState([
    {id: 0, name: "홍길동", content: "안녕하세요!" },
    {id: 1, name: "김철수", content: "반갑습니다!" },
    {id: 2, name: "이영희", content: "좋은 하루 되세요!" }
  ]);
  return(
    <div>
      <form>
        <input type = "text" id = "commentbar" placeholder = "이름"></input>
        <p>
          <input type = "text" id = "content" placeholder = "댓글을 입력하세요"></input>
          <input type = "button" value = "전송" onClick = {() => {
            setComments([...comments, {id: comments.length, name: document.getElementById("commentbar").value, content: document.getElementById("content").value}]);
          }}></input>
        </p>
      </form>

      <div>{/** 댓글 목록 */}
        {comments.slice().reverse().map((comment) =>{
          return(
            <div className = "comment">
              <p>{comment.name} : {comment.content}</p>
            </div>
          )
        })}
      </div>
    </div>
  )
}


export default function HomePage() {



    return(
        <div>
          <div className = "backgroundimg"><h2>타임 데빌</h2></div>
          
          <h3>게임 설명</h3>
          <p className = "description">1. '테스트용'이라는 용어는 주로 소프트웨어 개발 및 품질 보증 과정에서 사용됩니다. 이는 특정 기능이나 시스템이 제대로 작동하는지 확인하기 위한 테스트를 수행하는 것을 의미합니다. 개발자와 QA 엔지니어는 다양한 테스트를 통해 코드의 안정성, 성능, 보안성을 검토하며, 사용자 경험을 개선하기 위해 피드백을 받습니다. 이 과정에서 '테스트용' 데이터나 환경이 필요하며, 실제 사용자 정보와는 분리되어 있어야 합니다. 이러한 테스트는 최종 제품이 사용자의 기대에 부응할 수 있도록 보장하는 중요한 단계입니다.</p>
          <Comment></Comment>
        </div>
    )



}