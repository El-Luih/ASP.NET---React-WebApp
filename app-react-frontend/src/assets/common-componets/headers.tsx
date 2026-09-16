import LinkedImg from "../utilities-assets/linked-img";
import type { UserProfile } from "../account-assets/profile-utilities";
import { HomeNav } from "./navs";
import HamburgerButton from "../utilities-assets/hamburger-button";
import { useState } from "react";

type HeaderProps = {
    user: UserProfile;
}

function Header({ user }: HeaderProps) {
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false);

    return <header id="global-header">
        <LinkedImg target="/" source="/images/appLogo.svg" imageID="headerLogo" altText="Logo of Softia"/>
        <img src="/images/appBanner.png" />
        
        <div className="login-section">
            {user.isAuthenticated ? (
                <>  
                    <LinkedImg
                        target={user.role ===
                            'Student' ? "/student/dashboard/profile"
                            : "/instructor/dashboard/profile"}
                        source="/images/placeholderPFP.svg"
                        imageID="pfp"
                        altText="Go to Profile" />
                    <span className="user-greeting">Hi, {user.username}</span>
                    <a
                        className="button global-portal"
                        href={user.role === 'Student' ? "/student/dashboard" : "/instructor/dashboard"}>
                        Go to Portal
                    </a>
                </>
            ) : (
                <a href="/login" className="button global-login">Log In</a>
            )}
        </div>
        <HamburgerButton
            isOpen={isMenuOpen}
            setIsOpen={setIsMenuOpen}
            ariaControlsId="global-nav"
            menuClassName="Global"
        />
        <HomeNav navId="global-nav" isVisible={isMenuOpen} />
    </header>
}

export default Header;