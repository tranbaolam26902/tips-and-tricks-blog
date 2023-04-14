// Libraries
import { useRef } from 'react';
import { Button, Form } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

export default function AuthorFilterPane({ setKeyword, setYear, setMonth }) {
	// Component's refs
	const keywordRef = useRef();
	const yearRef = useRef();
	const monthRef = useRef();

	// Component's event handlers
	const handleFilterAuthors = (e) => {
		e.preventDefault();
		setKeyword(keywordRef.current.value);
		setYear(yearRef.current.value);
		setMonth(monthRef.current.value);
	};

	const handleClearFilter = () => {
		setKeyword('');
		setYear('');
		setMonth('');
		keywordRef.current.value = '';
		yearRef.current.value = '';
		monthRef.current.value = '';
	};

	return (
		<Form
			method='get'
			onSubmit={handleFilterAuthors}
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
				<Form.Label className='visually-hidden'>Nhập năm</Form.Label>
				<Form.Control
					ref={yearRef}
					type='text'
					placeholder='Nhập năm...'
					name='year'
				/>
			</Form.Group>
			<Form.Group className='col-auto'>
				<Form.Label className='visually-hidden'>Tháng</Form.Label>
				<Form.Select ref={monthRef} title='Tháng' name='month'>
					<option value=''>-- Chọn tháng --</option>
					{months.map((month) => (
						<option key={month} value={month}>
							Tháng {month}
						</option>
					))}
				</Form.Select>
			</Form.Group>
			<Form.Group className='col-auto'>
				<Button variant='primary' type='submit'>
					Tìm/Lọc
				</Button>
				<Button variant='warning mx-2' onClick={handleClearFilter}>
					Bỏ lọc
				</Button>
				<Link to='/admin/authors/edit' className='btn btn-success'>
					Thêm mới
				</Link>
			</Form.Group>
		</Form>
	);
}
