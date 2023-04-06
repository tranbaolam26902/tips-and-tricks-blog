import { Link } from 'react-router-dom';
import { Navbar, Nav } from 'react-bootstrap';

import styles from '../../styles/layout.module.css';

export default function Navigation() {
	return (
		<Navbar
			collapseOnSelect
			expand='sm'
			bg='white'
			variant='light'
			className={`${styles.header} border-bottom`}
		>
			<div className='container-fluid mx-4'>
				<Navbar.Brand>
					<Link to='/'>
						<img
							src='/assets/images/logo-text.png'
							alt='logo'
							style={{ height: 40 }}
						/>
					</Link>
				</Navbar.Brand>
				<Navbar.Toggle aria-controls='responsive-navbar-nav' />
				<Navbar.Collapse
					id='responsive-navbar-nav'
					className='d-sm-inline-flex justify-content-between'
				>
					<Nav className='mr-auto flex-grow-1'>
						<Nav.Item>
							<Link
								to='/admin/categories'
								className='nav-link text-dark'
							>
								Chủ đề
							</Link>
						</Nav.Item>
						<Nav.Item>
							<Link
								to='/admin/authors'
								className='nav-link text-dark'
							>
								Tác giả
							</Link>
						</Nav.Item>
						<Nav.Item>
							<Link
								to='/admin/tags'
								className='nav-link text-dark'
							>
								Thẻ
							</Link>
						</Nav.Item>
						<Nav.Item>
							<Link
								to='/admin/posts'
								className='nav-link text-dark'
							>
								Bài viết
							</Link>
						</Nav.Item>
						<Nav.Item>
							<Link
								to='/admin/comments'
								className='nav-link text-dark'
							>
								Bình luận
							</Link>
						</Nav.Item>
					</Nav>
				</Navbar.Collapse>
			</div>
		</Navbar>
	);
}
