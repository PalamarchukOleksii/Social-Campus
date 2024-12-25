import React, { createContext, useState, useContext } from "react";
import PropTypes from "prop-types";

const CreateItemContext = createContext();

export function CreateItemContextProvider({ children }) {
  const [isCreatePublicationOpen, setIsCreatePublicationOpen] = useState(false);
  const [isCreateCommentOpen, setIsCreateCommentOpen] = useState(false);

  const openCreatePublication = () => {
    setIsCreatePublicationOpen(true);
    document.body.classList.add("no-scroll");
  };

  const closeCreatePublication = () => {
    setIsCreatePublicationOpen(false);
    document.body.classList.remove("no-scroll");
  };

  const openCreateComment = () => {
    setIsCreateCommentOpen(true);
    document.body.classList.add("no-scroll");
  };

  const closeCreateComment = () => {
    setIsCreateCommentOpen(false);
    document.body.classList.remove("no-scroll");
  };

  return (
    <CreateItemContext.Provider
      value={{
        isCreatePublicationOpen,
        openCreatePublication,
        closeCreatePublication,
        isCreateCommentOpen,
        openCreateComment,
        closeCreateComment,
      }}
    >
      {children}
    </CreateItemContext.Provider>
  );
}

CreateItemContextProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export function useCreateItem() {
  return useContext(CreateItemContext);
}
