import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Publication from "../../components/publication/Publication";
import NavItem from "../../components/navItem/NavItem";
import PublicationDetailItems from "../../utils/consts/PublicationDetailItems";
import "./PublicationDetail.css";
import CreateComment from "../../components/createComment/CreateComment";
import getMaxCommentId from "../../utils/helpers/GetMaxCommentId";
import CommentReplyManager from "../../components/comment/CommentReplyManager";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const GET_PUBLICTION_URL = "/api/publications/";

function PublicationDetail() {
  const { id } = useParams();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

  const [publication, setPublication] = useState(null);
  const [loading, setLoading] = useState(true);
  const [hoveredIcon, setHoveredIcon] = useState("");
  const navigate = useNavigate();
  const [comments, setComments] = useState([]);

  useEffect(() => {
    const fetchPublication = async () => {
      try {
        setLoading(true);
        const response = await axios.get(`${GET_PUBLICTION_URL}${id}`);
        setPublication(response.data);
      } catch (error) {
        console.error("Failed to fetch publication:", error);
        setPublication(null);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchPublication();
    }
  }, [id, axios]);

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
      {loading ? (
        <></>
      ) : !publication ? (
        <h1 className="not-found-text general-text">Publication not found</h1>
      ) : (
        <>
          <Publication publication={publication} addCreateOpen={false} />
          <CreateComment
            user={auth.shortUser}
            comments={comments}
            setComments={setComments}
            getMaxCommentId={getMaxCommentId}
          />
          {comments?.length ? (
            <>
              <h2 className="comment-section-text general-text">Comments</h2>
              <div className="comments-section">
                {comments.map((comment) => (
                  <CommentReplyManager
                    key={comment.id}
                    comment={comment}
                    currentUser={auth.shortUser}
                    comments={comments}
                    setComments={setComments}
                  />
                ))}
              </div>
            </>
          ) : (
            <h2 className="general-text no-comments">No comments available</h2>
          )}
        </>
      )}
    </div>
  );
}

export default PublicationDetail;
