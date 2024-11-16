import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import Comment from "../../components/comment/Comment";
import publicationDetailsData from "../../data/userData.json";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";
import "./PublicationDetail.css";
import CreateComment from "../../components/createComment/CreateComment";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import login from "../../utils/consts/AuthUserLogin";

function PublicationDetail() {
  const { id } = useParams();
  const [publication, setPublication] = useState(null);
  const [loading, setLoading] = useState(true);
  const [hoveredIcon, setHoveredIcon] = useState("");
  const navigate = useNavigate();
  const [comments, setComments] = useState([]);
  const [currentUser, setCurrentUser] = useState(null);

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        const currentUser = publicationDetailsData.find(
          (user) => user.login === login
        );

        setCurrentUser(currentUser);

        const foundPublication = publicationDetailsData
          .flatMap((user) => user.publications)
          .find((pub) => pub.id === parseInt(id));

        if (foundPublication) {
          const foundUser = publicationDetailsData.find((user) =>
            user.publications.some((pub) => pub.id === foundPublication.id)
          );

          const publication = {
            id: foundPublication.id,
            description: foundPublication.description,
            imageUrl: foundPublication.imageUrl,
            creationTime: foundPublication.creationTime,
            likesCount: foundPublication.likesCount,
            comments: foundPublication.comments,
            username: foundUser.username,
            login: foundUser.login,
            profileImage: foundUser.profileImage,
          };

          setPublication(publication);
          setComments(foundPublication.comments);
        }
      } catch (error) {
        console.error("Error fetching publication:", error);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchPublication();
      window.scrollTo(0, 0);
    }
  }, [id]);

  if (loading) return <></>;
  if (!publication)
    return (
      <h1 className="not-found-text general-text">Publication not found</h1>
    );

  return (
    <div className="publication-detail-container">
      <div className="top-section">
        <NavItem
          label={PublicationDetailItems.label}
          inactiveIcon={PublicationDetailItems.inactiveIcon}
          activeIcon={PublicationDetailItems.activeIcon}
          hoveredIcon={hoveredIcon}
          setHoveredIcon={setHoveredIcon}
          onClick={() => navigate(-1)}
        />
        <h1 className="general-text">Publication</h1>
      </div>
      <Publication publication={publication} />
      <CreateComment
        user={currentUser}
        comments={comments}
        setComments={setComments}
        getMaxCommentId={getMaxCommentId}
      />
      {comments.length > 0 && (
        <>
          <h2 className="comment-section-text general-text">Comments</h2>
          <div className="comments-section">
            {comments.map((comment) => (
              <Comment
                key={comment.id}
                username={comment.username}
                login={comment.login}
                profileImage={comment.profileImage}
                text={comment.text}
                likeCount={comment.likeCount}
                creationTime={comment.creationTime}
              />
            ))}
          </div>
        </>
      )}
      {comments.length === 0 && (
        <h2 className="general-text no-comments">No comments available</h2>
      )}
    </div>
  );
}

export default PublicationDetail;
