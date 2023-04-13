// Libraries
import { useRef } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Link } from 'react-router-dom';

export default function CategoryFilterPane({ setKeyword, setShowOnMenu }) {
	// Component's refs
	const keywordRef = useRef();
	const showOnMenuRef = useRef();

	// Component's event handlers
	const handleFilterPosts = (e) => {
		e.preventDefault();
		setKeyword(keywordRef.current.value);
		setShowOnMenu(showOnMenuRef.current.checked);
	};

	const handleClearFilter = () => {
		setKeyword('');
		setShowOnMenu(false);
		keywordRef.current.value = '';
		showOnMenuRef.current.checked = false;
	};

	return (
		<Form
			method='get'
			onSubmit={handleFilterPosts}
			className='row gx-3 gy-2 align-items-center py-2'
		>
			<Form.Group className='col-auto'>
				<Form.Label className='visually-hidden'>Từ khóa</Form.Label>
				<Form.Control
					ref={keywordRef}
					type='text'
					placeholder='Nhập từ khóa...'
					name='keyword'
				/>
			</Form.Group>
			<Form.Group className='col-auto'>
				<input id='show-on-menu' type='checkbox' ref={showOnMenuRef} />
				<label htmlFor='show-on-menu' className='ms-1'>
					Hiển thị trên menu
				</label>
			</Form.Group>
			<Form.Group className='col-auto'>
				<Button variant='primary' type='submit'>
					Tìm/Lọc
				</Button>
				<Button variant='warning mx-2' onClick={handleClearFilter}>
					Bỏ lọc
				</Button>
				<Link to='/admin/categories/edit' className='btn btn-success'>
					Thêm mới
				</Link>
			</Form.Group>
		</Form>
	);
}
