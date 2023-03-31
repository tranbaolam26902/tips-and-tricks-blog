import { useRef, useState } from 'react';

import { sendComment } from '../services/comments';

export default function CommentForm({ postId }) {
	// Component's states
	const [name, setName] = useState('');
	const [description, setDescription] = useState('');

	// Component's event handlers
	const handleComment = async (e) => {
		e.preventDefault();
		if (name.trim() && description.trim()) {
			const data = await sendComment({
				id: postId,
				name: name.trim(),
				description: description.trim(),
			});
			if (data) {
				window.alert(data);
				setName('');
				setDescription('');
			} else window.alert('Gửi thất bại!');
		}
	};

	return (
		<div className='p-4 border border-1 border-gray rounded'>
			<h5>Gửi bình luận về bài viết</h5>
			<form onSubmit={handleComment}>
				<div className='mb-3'>
					<div className='d-flex justify-content-between'>
						<label htmlFor='name' className='form-label'>
							Tên
						</label>
						<span className='text-secondary fst-italic fs-6'>
							(Bắt buộc)
						</span>
					</div>
					<input
						value={name}
						type='text'
						onChange={(e) => setName(e.target.value)}
						name='name'
						className='form-control'
						id='name'
						required
					/>
				</div>
				<div className='mb-3'>
					<div className='d-flex justify-content-between'>
						<label htmlFor='content' className='form-label'>
							Nội dung
						</label>
						<span className='text-secondary fst-italic fs-6'>
							(Bắt buộc)
						</span>
					</div>
					<textarea
						value={description}
						className='form-control'
						onChange={(e) => setDescription(e.target.value)}
						name='description'
						id='content'
						rows='4'
						required
					></textarea>
				</div>
				<input type='hidden' name='postId' />
				<button
					type='submit'
					className='btn btn-primary mt-3 px-5 py-2'
				>
					Gửi
				</button>
			</form>
		</div>
	);
}
