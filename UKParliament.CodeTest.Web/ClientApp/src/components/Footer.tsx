import React from "react";

const Footer: React.FC = () => {
  return (
    <footer style={{ background: "#1a1a1a", color: "white", textAlign: "center", padding: "10px 0", fontSize: "14px" }}>
      Copyright © {new Date().getFullYear()}
    </footer>
  );
};

export default Footer;
