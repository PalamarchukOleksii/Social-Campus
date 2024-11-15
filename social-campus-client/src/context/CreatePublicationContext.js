import React, { createContext, useState, useContext } from "react";
import PropTypes from "prop-types";

const CreatePublicationContext = createContext();

export function CreatePublicationContextProvider({ children }) {
  const [isOpen, setIsOpen] = useState(false);

  const open = () => setIsOpen(true);
  const close = () => setIsOpen(false);

  return (
    <CreatePublicationContext.Provider value={{ isOpen, open, close }}>
      {children}
    </CreatePublicationContext.Provider>
  );
}

CreatePublicationContextProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export function useCreatePublication() {
  return useContext(CreatePublicationContext);
}
