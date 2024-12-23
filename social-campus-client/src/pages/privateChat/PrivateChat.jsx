import React, { useState, useEffect, useRef } from "react";
import "./PrivateChat.css";
import { useParams, useNavigate } from "react-router-dom";
import {
  IoArrowBackCircleOutline,
  IoArrowBackCircle,
  IoSend,
  IoSendOutline,
  IoClose,
  IoCloseOutline,
} from "react-icons/io5";
import userData from "../../data/userData.json";
import MessageBubble from "../../components/messageBubble/MessageBubble";
import messagesData from "../../data/chatsData.json";
import authLogin from "../../utils/consts/AuthUserLogin";

function PrivateChat() {
  const { login } = useParams();
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [isSendHovered, setIsSendHovered] = useState(false);
  const navigate = useNavigate();
  const [chatUser, setChatUser] = useState(null);
  const [authUser, setAuthUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [messageInput, setMessageInput] = useState("");
  const [messages, setMessages] = useState([]);
  const [replyToMessage, setReplyToMessage] = useState(null);
  const [isRemoveReplyHovered, setIsRemoveReplyHovered] = useState(false);
  const [countOfNewMessages, setCountOfNewMessages] = useState(0);

  const messageContainerRef = useRef(null);

  useEffect(() => {
    if (messageContainerRef.current) {
      messageContainerRef.current.scrollTop =
        messageContainerRef.current.scrollHeight;
    }
  }, [messages]);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      const authUser = userData.find((user) => user.login === authLogin);

      setAuthUser(authUser || null);
      setChatUser(foundUser || null);
      setLoading(false);
    };

    fetchUserData();
  }, [login]);

  useEffect(() => {
    const filteredMessages = messagesData.filter(
      (message) =>
        message.sender.login === login || message.receiver.login === login
    );
    setMessages(filteredMessages);
  }, [login]);

  const handleSendMessage = () => {
    if (messageInput.trim() === "") return;

    setCountOfNewMessages((prev) => (prev += 1));

    const newMessage = {
      id: messagesData.length + countOfNewMessages + 1,
      sender: {
        id: authUser.id,
        username: authUser.username,
        login: authUser.login,
        profileImage: authUser.profileImage,
      },
      receiver: chatUser,
      text: messageInput,
      timestamp: new Date().toISOString(),
      likes: 0,
      repliesCount: 0,
      replyTo: replyToMessage,
    };

    setMessages([...messages, newMessage]);
    setMessageInput("");
    setReplyToMessage(null);
    const textarea = document.querySelector(".message-input");
    if (textarea) {
      textarea.style.height = "auto";
    }
  };

  const sendMessageOnEnter = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
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
        <h1 className="chat-user-name general-text">{chatUser.username}</h1>
      </div>
      <div className="message-container" ref={messageContainerRef}>
        {messages.length === 0 ? (
          <div className="no-messages">
            <h2 className="no-messages-text general-text">
              No messages yet.
              <br /> Start a conversation!
            </h2>
          </div>
        ) : (
          messages.map((message, index) => (
            <MessageBubble
              key={index}
              messageId={message.id}
              profileImage={message.sender.profileImage}
              username={message.sender.username}
              login={message.sender.login}
              text={message.text}
              timestamp={message.timestamp}
              likeCount={message.likes}
              replies={message.repliesCount}
              handleReplyClick={() => setReplyToMessage(message)}
              replyTo={message.replyTo}
            />
          ))
        )}
      </div>
      <div className="message-input-container">
        {replyToMessage && (
          <div className="reply-preview">
            <div className="reply-preview-info">
              <h4 className="message-sender-name general-text">
                Reply to {replyToMessage.sender.username}
              </h4>
              <h4 className="message-text general-text">
                {replyToMessage.text}
              </h4>
            </div>
            <div
              className="remove-reply remove-image-icon general-text"
              onMouseEnter={() => setIsRemoveReplyHovered(true)}
              onMouseLeave={() => setIsRemoveReplyHovered(false)}
              onClick={() => setReplyToMessage(null)}
            >
              {isRemoveReplyHovered ? <IoClose /> : <IoCloseOutline />}
            </div>
          </div>
        )}
        <div className="message-preview">
          <textarea
            className="message-input"
            placeholder="Type a message..."
            value={messageInput}
            onChange={(e) => setMessageInput(e.target.value)}
            onInput={(e) => {
              e.target.style.height = "auto";
              e.target.style.height = `${e.target.scrollHeight}px`;
            }}
            onKeyDown={sendMessageOnEnter}
            required
          />
          {messageInput.trim() && (
            <div
              className="send-icon general-text"
              onMouseEnter={() => setIsSendHovered(true)}
              onMouseLeave={() => setIsSendHovered(false)}
              onClick={handleSendMessage}
            >
              {isSendHovered ? <IoSend /> : <IoSendOutline />}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default PrivateChat;
