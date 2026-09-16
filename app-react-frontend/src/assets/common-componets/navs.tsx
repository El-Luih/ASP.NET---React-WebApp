//CONTAINS DIFFERENT NAV ELEMENTS

//Header Global Nav
type NavProps = {
    navId: string,
    isVisible: boolean,
}
function HomeNav({ navId, isVisible }: NavProps) {
    return <nav id={navId} className={isVisible ? "visible" : "hidden"}>
        <ul>
            <li><a href="/">Home Page</a></li>
            <li><a href="/about-us">About Us</a></li>
            <li><a href="/explore">Explore Courses</a></li>
        </ul>
    </nav>
}



function StudentSideBar() { }

function InstructorSideBar() { }

export {HomeNav}