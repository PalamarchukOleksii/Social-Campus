import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import Comment from "../../components/comment/Comment";
import publicationDetailsData from "../../data/userData.json";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";
import "./PublicationDetail.css";

function PublicationDetail() {
  const { id } = useParams();
  const [publication, setPublication] = useState(null);
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [hoveredIcon, setHoveredIcon] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        const foundPublication = publicationDetailsData
          .flatMap((user) => user.publications)
          .find((pub) => pub.id === parseInt(id));

        if (foundPublication) {
          const foundUser = publicationDetailsData.find((user) =>
            user.publications.some((pub) => pub.id === foundPublication.id)
          );
          setPublication(foundPublication);
          setUser(foundUser);
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
      <Publication
        publicationId={publication.id}
        username={user.username}
        login={user.login}
        profileImage={user.profileImage}
        creationTime={publication.creationTime}
        imageUrl={publication.imageUrl}
        description={publication.description}
        likesCount={publication.likesCount}
        commentsCount={publication.comments.length}
      />
      <h2 className="comment-section-text general-text">Comments</h2>
      <div className="comments-section">
        {publication.comments.length > 0 ? (
          publication.comments.map((comment, index) => (
            <Comment
              key={index}
              username={comment.username}
              login={comment.login}
              text={comment.text}
              likeCount={comment.likeCount}
              creationTime={comment.creationTime}
            />
          ))
        ) : (
          <p>No comments available</p>
        )}
      </div>
    </div>
  );
}

export default PublicationDetail;
