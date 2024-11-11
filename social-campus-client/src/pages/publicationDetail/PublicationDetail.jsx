import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import Comment from "../../components/comment/Comment";
import "./PublicationDetail.css";
import publicationDetailsData from "../../data/userData.json";

function PublicationDetail() {
  const { id } = useParams();
  const [publication, setPublication] = useState(null);
  const [user, setUser] = useState(null); // Store the user object
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        // Find the publication by id and the user who posted it
        const foundPublication = publicationDetailsData
          .flatMap((user) => user.publications)
          .find((pub) => pub.id === parseInt(id)); // Ensure 'id' is parsed to an integer

        if (foundPublication) {
          // Find the user who posted the publication
          const foundUser = publicationDetailsData.find((user) =>
            user.publications.some((pub) => pub.id === foundPublication.id)
          );
          setPublication(foundPublication);
          setUser(foundUser); // Set the user state
        }
      } catch (error) {
        console.error("Error fetching publication:", error);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchPublication();
    }
  }, [id]);

  if (loading) return <p>Loading publication...</p>;
  if (!publication) return <p>Publication not found.</p>;

  return (
    <div className="publication-detail-container">
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
      <h3 className="comment-section-text">Comments:</h3>
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
