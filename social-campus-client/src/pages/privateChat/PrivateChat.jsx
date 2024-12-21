import React, { useState, useEffect } from "react";
import "./PrivateChat.css";
import { useParams, useNavigate } from "react-router-dom";
import { IoArrowBackCircleOutline, IoArrowBackCircle } from "react-icons/io5";
import ShortProfile from "../../components/shortProfile/ShortProfile";
import userData from "../../data/userData.json";

function PrivateChat() {
  const { login } = useParams();
  const [isExitHovered, setIsExitHovered] = useState(false);
  const navigate = useNavigate();
  const [chatUser, setChatUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [messageInput, setMessageInput] = useState("");
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      setChatUser(foundUser || null);
      setLoading(false);
    };

    fetchUserData();
  }, [login]);

  const handleSendMessage = () => {
    if (messageInput.trim() === "") return;
    setMessages([...messages, { text: messageInput, sender: "me" }]);
    setMessageInput("");
    const textarea = document.querySelector(".message-input");
    if (textarea) {
      textarea.style.height = "auto";
    }
  };

  if (loading) {
    return <></>;
  }

  if (!chatUser) {
    return (
      <div className="private-chat-container">
        <div className="top-bar">
          <div
            className="close-creation-icon general-text"
            onMouseEnter={() => setIsExitHovered(true)}
            onMouseLeave={() => setIsExitHovered(false)}
            onClick={() => navigate(-1)}
          >
            {isExitHovered ? (
              <IoArrowBackCircle />
            ) : (
              <IoArrowBackCircleOutline />
            )}
            <span className="general-text back-text">Back</span>
          </div>
        </div>
        <div className="error-message">
          <h1 className="not-found-text general-text">
            Chat with user {login} not found
          </h1>
        </div>
      </div>
    );
  }

  return (
    <div className="private-chat-container">
      <div className="top-bar">
        <div
          className="close-creation-icon general-text"
          onMouseEnter={() => setIsExitHovered(true)}
          onMouseLeave={() => setIsExitHovered(false)}
          onClick={() => navigate(-1)}
        >
          {isExitHovered ? <IoArrowBackCircle /> : <IoArrowBackCircleOutline />}
          <span className="general-text back-text">Back</span>
        </div>
        <ShortProfile
          username={chatUser.username}
          login={chatUser.login}
          profileImage={chatUser.profileImage}
        />
      </div>
      <div className="message-container">
        {messages.map((message, index) => (
          <div
            key={index}
            className={`message ${
              message.sender === "me" ? "sent" : "received"
            }`}
          >
            {message.text}
          </div>
        ))}
      </div>
      <div className="message-input-container">
        <textarea
          className="message-input"
          placeholder="Type a message..."
          value={messageInput}
          onChange={(e) => setMessageInput(e.target.value)}
          onInput={(e) => {
            e.target.style.height = "auto";
            e.target.style.height = `${e.target.scrollHeight}px`;
          }}
          required
        />
        {messageInput.trim() && (
          <button className="send-button" onClick={handleSendMessage}>
            Send
          </button>
        )}
      </div>
    </div>
  );
}

export default PrivateChat;
