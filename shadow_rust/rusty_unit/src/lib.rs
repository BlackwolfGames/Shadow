#[derive(Debug)]
pub enum Constraint<T> {
    EqualTo(T),
    Not(Box<Constraint<T>>),
    GreaterThan(T),
    LessThan(T),
}

impl<T> Constraint<T> {
    fn not(self) -> Self {
        Constraint::Not(Box::new(self))
    }
}

pub mod is {
    use super::Constraint;
    use super::Constraint::*;

    pub mod not {
        use super::*;

        pub fn equal_to<T>(value: T) -> Constraint<T> {
            super::equal_to(value).not()
        }

        pub fn greater_than<T>(value: T) -> Constraint<T> {
            super::greater_than(value).not()
        }

        pub fn less_than<T>(value: T) -> Constraint<T> {
            super::less_than(value).not()
        }
    }

    pub fn equal_to<T>(value: T) -> Constraint<T> {
        EqualTo(value)
    }

    pub fn greater_than<T>(value: T) -> Constraint<T> {
        GreaterThan(value)
    }

    pub fn less_than<T>(value: T) -> Constraint<T> {
        LessThan(value)
    }
}

impl<T: PartialOrd> Constraint<T> {
    pub fn check(&self, value: &T) -> bool {
        match self {
            Self::EqualTo(ref expected) => value == expected,
            Self::Not(constraint) => !constraint.check(value),
            Self::GreaterThan(ref expected) => value > expected,
            Self::LessThan(ref expected) => value < expected,
        }
    }
}
#[macro_export]
macro_rules! assert_that {
    ($value:expr, $constraint:expr $(,)?) => {
        match (&$value, &$constraint) {
            (value, constraint) => {
                if !constraint.check(value) {
                    panic!("Expected {:?}, but got {:?}", constraint, value);
                }
            }
        }
    };
}
