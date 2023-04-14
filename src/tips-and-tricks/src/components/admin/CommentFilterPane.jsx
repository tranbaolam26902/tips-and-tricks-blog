import { useEffect, useRef, useState } from 'react';
import { Button, Form } from 'react-bootstrap';

import { getPosts } from '../../services/posts';

const months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

export default function CommentFilterPane({
	setKeyword,
	setPostId,
	setYear,
	setMonth,
	setIsNotApproved,
}) {
	// Component's states
	const [posts, setPosts] = useState([]);

	// Component's refs
	const keywordRef = useRef();
	const postIdRef = useRef();
	const yearRef = useRef();
	const monthRef = useRef();
	const isNotApprovedRef = useRef();

	// Component's event handlers
	const handleFilterComments = (e) => {
		e.preventDefault();
		setKeyword(keywordRef.current.value);
		setPostId(postIdRef.current.value);
		setYear(yearRef.current.value);
		setMonth(monthRef.current.value);
		setIsNotApproved(isNotApprovedRef.current.checked);
	};

	const handleClearFilter = () => {
		setKeyword('');
		setPostId('');
		setYear('');
		setMonth('');
		setIsNotApproved(false);
		keywordRef.current.value = '';
		postIdRef.current.value = '';
		yearRef.current.value = '';
		monthRef.current.value = '';
		isNotApprovedRef.current.checked = false;
	};

	useEffect(() => {
		fetchData();

		async function fetchData() {
			const posts = await getPosts();
			if (posts) setPosts(posts);
		}
	}, []);

	return (
		<Form
			method='get'
			onSubmit={handleFilterComments}
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
				<Form.Label className='visually-hidden'>Bài viết</Form.Label>
				<Form.Select ref={postIdRef} title='Tác giả' name='postId'>
					<option value=''>-- Chọn bài viết --</option>
					{posts.length > 0 &&
						posts.map((post) => (
							<option key={post.id} value={post.id}>
								{post.title}
							</option>
						))}
				</Form.Select>
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
				<input
					id='isNotApproved'
					type='checkbox'
					ref={isNotApprovedRef}
				/>
				<label htmlFor='isNotApproved' className='ms-1'>
					Chưa phê duyệt
				</label>
			</Form.Group>
			<Form.Group className='col-auto'>
				<Button variant='primary' type='submit'>
					Tìm/Lọc
				</Button>
				<Button variant='warning mx-2' onClick={handleClearFilter}>
					Bỏ lọc
				</Button>
			</Form.Group>
		</Form>
	);
}
