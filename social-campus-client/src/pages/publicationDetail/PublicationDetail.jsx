import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import Comment from "../../components/comment/Comment";
import "./PublicationDetail.css";
import publicationDetailsData from "../../data/publicationDetailsData.json";

function PublicationDetail() {
  const { id } = useParams();
  const [publication, setPublication] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        setPublication(publicationDetailsData);
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
        username={publication.login}
        login={publication.login}
        creationTime={publication.creationTime}
        imageUrl={publication.imageUrl}
        description={publication.description}
      />

      <h3>Comments:</h3>
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
